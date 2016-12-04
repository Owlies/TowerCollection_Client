using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Sproto;
using SprotoType;

namespace Owlies.Core {
    public enum eMessageRequestType {
        ChangeEvent = 1, // No Server Response
        ApiCall = 2, // Server Response
    }

    public class ConnectionManager : Singleton<ConnectionManager> {
        private int session;
        public byte[] sendBuffer;

        public int sendBufferSize;
        private const int MAX_BUFFER_SIZE = 1 << 16;

        void Start() {
            
        }

        ConnectionManager() {
            this.session = 300;
            this.sendBuffer = new byte[MAX_BUFFER_SIZE];
        }

        public void serialize(SprotoTypeBase sprotoObject, eMessageRequestType messageType) {
            string messageName = sprotoObject.GetType().Name;
            int totalSize = 9 + messageName.Length;

            this.sendBuffer[2] = (byte)messageType;

            this.sendBuffer[3] = (byte)(this.session >> 24);
            this.sendBuffer[4] = (byte)(this.session >> 16);
            this.sendBuffer[5] = (byte)(this.session >> 8);
            this.sendBuffer[6] = (byte)(this.session);

            this.sendBuffer[7] = (byte)(messageName.Length >> 8);
            this.sendBuffer[8] = (byte)(messageName.Length);
            char [] a = messageName.ToCharArray();
            System.Buffer.BlockCopy(messageName.ToCharArray(), 0, this.sendBuffer, 9, messageName.Length);
            byte [] encodedMessage = sprotoObject.encode();

            System.Buffer.BlockCopy(encodedMessage, 0, this.sendBuffer, totalSize, encodedMessage.Length);
            totalSize += encodedMessage.Length;

            this.sendBuffer[0] = (byte)(totalSize >> 8);
            this.sendBuffer[1] = (byte)(totalSize);

            this.sendBufferSize = totalSize;
        }

        public SprotoTypeBase deserialize(byte[] package) {
            int totalSize = package[0] << 8 | package[1];
            int session = package[2] << 24 | package[3] << 16 | package[4] << 8 | package[5];
            int messageNameSize = package[6] << 8 | package[7];
            char [] chars = new char[messageNameSize];
            for(int i = 0; i < messageNameSize; ++i) {
                chars[i] = (char)package[i + 8];
            }
            string messageName = new string(chars);
            int messageSize = totalSize - 8 - messageNameSize;
            byte [] message = new byte[messageSize];
            Array.Copy(package, 8 + messageNameSize, message, 0, messageSize);
            if (messageName == "Person") {
                Person person = new Person(message);
                return person;
            }

            return null;
        }
        
    }
}