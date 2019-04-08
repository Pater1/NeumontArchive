
#include <stdlib.h>
#include <ctype.h>
#include <chrono>
#include <conio.h>

#include "RandUtils.h"

int RandInt(int max, int min) {
	if (max == min) return max;

	srand((unsigned int)(std::chrono::system_clock::now().time_since_epoch().count()));
	int r = rand();
	r %= (max - min);
	r += min;
	return r;
}

float RandFloat(float max, float min) {
	if (max == min) return max;

	srand((unsigned int)(std::chrono::system_clock::now().time_since_epoch().count()));
	int ra = rand();
	float nd = (float)ra / RAND_MAX;

	nd *= (max - min);
	nd += min;

	return nd;
}