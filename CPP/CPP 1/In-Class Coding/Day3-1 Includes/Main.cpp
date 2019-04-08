#include <iostream>
#include "../Common/DebugUtility.h"
#include "Func.h"


//int main() {
//int main(int argc, char** argv){
int main(int argc, char* argv[]){
	for (int i = 0; i < argc; i++) {
		p2(i, argv[i]);
	}

	int j = 5;
//	pn(j, 5, 'h', 6.05f, 500L, 4.28, "hi");

	Func(3.05f);

	return 0;
}