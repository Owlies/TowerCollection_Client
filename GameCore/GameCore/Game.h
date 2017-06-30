#pragma once

#include <vector>
#include "Object.h"

using namespace std;
class Game
{
public:
	int a = 1;
	int b = 2;
private:
	//vector<Object> 
};

extern "C" int GAMECORE_DllEXPORRT testFunc(Game g);
extern "C" int GAMECORE_DllEXPORRT addFunc(int a, int b);
