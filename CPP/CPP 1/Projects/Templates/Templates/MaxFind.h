#pragma once
#include<string>

inline int FindMax(int a, int b) {
	return (a >= b ? a : b);
}

inline double FindMax(double a, double b) {
	return (a >= b ? a : b);
}

inline std::string FindMax(std::string a, std::string b) {
	return (a >= b ? a : b);
}


template <typename T>
inline T  FindMaximum(T  a, T  b) {
	return (a >= b) ? a : b;
}

template <typename T>
inline T  FindMaximum(T* arry, int count) {
	T max = arry[0];
	if (count > 0) {
		for (int i = 1; i < count; i++) {
			max = FindMaximum(max, arry[i]);
		}
	}
	return max;
}

template <typename T, typename... Args>
inline T  FindMaximum(T first, T second, Args... vals) {
	return FindMaximum(FindMaximum(first, second), vals...);
}