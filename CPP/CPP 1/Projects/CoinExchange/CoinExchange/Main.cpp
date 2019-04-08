#include<iostream>
#include<string>;

/*
	cc->sc->gc->mb

	100 copper coins = 1 silver coin
	100 silver coins = 1 gold coin
	25 gold coins = 1 mithril bar

	100 copper coins = 10 pounds
	100 silver coins = 8 pounds
	100 gold coins = 5 pounds
	1 mithril bar = 1 pound
*/

const int ccToSc = 100, scToGc = 100, gcToMb = 25;
const float ccToLb = 10 / 100, scToLb = 8.0f / 100, gcToLb = 5.0f / 100, mbToLb = 1;

void ConvertCurrency(int* baseCurrencyCount, int* conversionCurrencyCount, int conversionRate) {
	while (*baseCurrencyCount >= ccToSc) {
		*baseCurrencyCount -= ccToSc;
		*conversionCurrencyCount += 1;
	}
}
void FullStackCurrencyConversion(int* cc, int* sc, int* gc, int* mb) {
	ConvertCurrency(cc, sc, ccToSc);
	ConvertCurrency(sc, gc, scToGc);
	ConvertCurrency(gc, mb, gcToMb);
}

float GetWeight(int* cc, int* sc, int* gc, int* mb) {
	return (*cc * ccToLb) + (*sc * scToLb) + (*gc * gcToLb) + (*mb * mbToLb);
}

void main() {
	std::string avdName = "";
	int cc = 0, sc = 0, gc = 0, mb = 0;

	std::cout << "Hello adventurer! What is your name? ";
	std::cin >> avdName;
	std::cout << "\n\n";

	std::cout << "Well met " << avdName << ", I will echange your coins.\n\n";

	std::cout << "How many Copper Coins do you have to exchange? ";
	std::cin >> cc;
	std::cout << "\n";

	std::cout << "How many Silver Coins do you have to exchange? ";
	std::cin >> sc;
	std::cout << "\n";

	std::cout << "How many Gold Coins do you have to exchange? ";
	std::cin >> gc;
	std::cout << "\n";

	std::cout << "Your pack weighs " << GetWeight(&cc, &sc, &gc, &mb) << "lb.\n\n";

	std::cout << "Converting coins...\n\n";
	FullStackCurrencyConversion(&cc, &sc, &gc, &mb);

	std::cout << "You have " << cc << " Copper Coins, " << sc << " Silver Coins, " << gc << " Gold Coins, and " << mb << " Mithril Bars.\n";
	
	std::cout << "Your pack weighs " << GetWeight(&cc, &sc, &gc, &mb) << "lb.\n\n";
}