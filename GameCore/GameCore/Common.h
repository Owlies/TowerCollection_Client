#pragma once

#define GAMECORE_DllEXPORRT   __declspec( dllexport )

enum OBJECT_TYPE
{
	OBJECT,
	UNIT,
	PROJECTILE,
	AREA_EFFECT,
};