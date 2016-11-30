using UnityEngine;
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
            int totalSize = 10 + messageName.Length;

            this.sendBuffer[3] = (byte)messageType;

            this.sendBuffer[4] = (byte)(this.session >> 24);
            this.sendBuffer[5] = (byte)(this.session >> 16);
            this.sendBuffer[6] = (byte)(this.session >> 8);
            this.sendBuffer[7] = (byte)(this.session);

            this.sendBuffer[8] = (byte)(messageName.Length >> 8);
            this.sendBuffer[9] = (byte)(messageName.Length);

            System.Buffer.BlockCopy(messageName.ToCharArray(), 0, this.sendBuffer, 10, messageName.Length);
            byte [] encodedMessage = sprotoObject.encode();

            System.Buffer.BlockCopy(encodedMessage, 0, this.sendBuffer, totalSize, encodedMessage.Length);
            totalSize += encodedMessage.Length;

            this.sendBuffer[0] = (byte)(totalSize >> 8);
            this.sendBuffer[1] = (byte)(totalSize);

            this.sendBufferSize = totalSize;
        }
        
    }
}