#include "Currency.h"

#include<iostream>

Currency::Currency(){}

Currency::Currency(int totalCoppers){
	SyncFromTotal(totalCoppers);
}

Currency::Currency(int copper, int silver, int gold, int mythril){
	AlterByCoinage(copper, silver, gold, mythril);
}


Currency::~Currency(){}

void Currency::SyncFromTotal(int total){
	int _not_myth = total % (ccToSc * scToGc * gcToMb);
	myth = (total - _not_myth) / (ccToSc * scToGc * gcToMb);
	total = _not_myth;

	int _not_gc = total % (ccToSc * scToGc);
	gld = (total - _not_gc) / (ccToSc * scToGc);
	total = _not_gc;

	int _not_slv = total % (ccToSc);
	silv = (total - _not_slv) / (ccToSc);
	total = _not_slv;

	cop = total;
}

int Currency::GetTotal() {
	return GetTotal(cop, silv, gld, myth);
}
int Currency::GetTotal(int copper, int silver, int gold, int mythril){
	return copper + 
					(ccToSc * (silver +
								(scToGc * (gold +
										(mythril * gcToMb
											))
								))
					);
}

void Currency::AlterByTotal(int totalDelta){
	if (-totalDelta > GetTotal()) throw std::exception("You can't afford that!");
	SyncFromTotal(GetTotal() + totalDelta);
}

void Currency::AlterByCoinage(int copperDelta, int silverDelta, int goldDelta, int mythrilDelta){
	int totalDelta = GetTotal(copperDelta, silverDelta, goldDelta, mythrilDelta);
	AlterByTotal(totalDelta);
}

std::ostream* Currency::PrintMoney(std::ostream * out){
	if (out == NULL) return out;
	*out << cop << " Coppers";
	if (silv > 0) *out << ", " << silv << " Silver";
	if (gld > 0) *out << ", " << gld << " Silver";
	if (myth > 0) *out << ", " << myth << " Silver";
	return out;
}
