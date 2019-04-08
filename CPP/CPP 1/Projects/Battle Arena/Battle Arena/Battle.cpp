#include "Battle.h"
#include "Party.h"
#include "RandUtils.h"
#include <iostream>

Battle::Battle(Party* partyA, Party* partyB, std::ostream * out) : Battle::Battle(partyA->GetPartyMembers(), partyB->GetPartyMembers(), out){
	refsParties = true;
	pA = partyA;
	pB = partyB;
}

Battle::Battle(std::vector<Character*>* partyA, std::vector<Character*>* partyB, std::ostream* out){
	A = partyA;
	B = partyB;

	cachedOut = out;
	if (out != NULL) {
		*out << "Today we have a battle between,\n	PartyA:\n";

		for (int i = 0; i < A->size(); i++) {
			*out << "		";
			(*A)[i]->CheckDead(A, B, out);
			*out << "\n";
		}

		*out << "\n	PartyB:\n";

		for (int i = 0; i < B->size(); i++) {
			*out << "		";
			(*B)[i]->CheckDead(B, A, out);
			*out << "\n";
		}

		*out << "Let the battle begin! \n\n";
	}
}


Battle::~Battle(){
	DisperseAwards();

	if (refsParties) {
		delete pA;
		delete pB;
	} else {
		for (int i = 0; i < A->size(); i++) {
			delete (*A)[i];
		}
		for (int i = 0; i < B->size(); i++) {
			delete (*B)[i];
		}

		delete A;
		delete B;
	}
}

void Battle::DisperseAwards() {
	// 1) determine which party won
	bool Adead = true;
	if (A->size() > 0) {
		for (int i = A->size() - 1; i >= 0; i--) {
			if (!(*A)[i]->CheckDead(A, B, cachedOut)) {
				Adead = false;
			}
			//5) revive dead characters
			if ((*A)[i] != NULL)
				(*A)[i]->Revive(A, B, cachedOut);
		}
	}

	if (cachedOut != NULL)
		*cachedOut << "\n";

	bool Bdead = true;
	if (B->size()) {
		for (int i = B->size() - 1; i >= 0; i--) {
			if (!(*B)[i]->CheckDead(B, A, cachedOut)) {
				Bdead = false;
			}
			//5) revive dead characters
			if ((*B)[i] != NULL)
				(*B)[i]->Revive(B, A, cachedOut);
		}
	}

	if (Adead != Bdead) {
		std::vector<Character*>* winning;
		std::vector<Character*>* losing;
		if (Adead) {
			winning = B;
			losing = A;
		} else {
			winning = A;
			losing = B;
		}

		//2) deduct x% of each character's $ who lost
			//2.5) total that amount
		int totalCoinDrop = 0;
		double totalXPDrop = 0;
		for (int i = 0; i < losing->size(); i++) {
			totalXPDrop += (*losing)[i]->XPDrop();

			int coinDrop = (int)((*losing)[i]->GetMoney().GetTotal() * RandFloat(0.1f, 0.5f));
			totalCoinDrop += coinDrop;
			coinDrop = (int)(coinDrop * 0.5);

			if (cachedOut != nullptr)
				*(*losing)[i]->FullName(cachedOut) << " drops " << coinDrop << " Coppers!\n\n";
			
			(*losing)[i]->GetMoney().AlterByTotal(-coinDrop);

			(*losing)[i]->GetEditLosses(1);
		}

		//3) evenly distribute that $ among the characters who won
		for (int i = 0; i < winning->size(); i++) {
			int coinGain = totalCoinDrop / (int)winning->size();
			(*winning)[i]->GetMoney().AlterByTotal(coinGain);

			double XPGain = totalXPDrop / winning->size();
			XPGain = std::ceil(XPGain);
			(*winning)[i]->XPPickup(XPGain);

			if (cachedOut != nullptr) {
				*(*winning)[i]->FullName(cachedOut) << " picks up " << coinGain << " Coppers!\n";
				*(*winning)[i]->FullName(cachedOut) << " gains " << XPGain << " XP!\n\n";
			}

			//4) add one win to the winning party's characters' win counter
			(*winning)[i]->GetEditWins(1);
		}
	}
}

bool Battle::Turn(std::ostream* out) {
	turnCount++;
	if(out != NULL)
		*out << "\nTurn: " << turnCount << '\n';

	for (int i = 0; i < A->size(); i++) {
		(*A)[i]->ThrowAttack(-1, A, B, (out == NULL? NULL: out));
		if (out != NULL)
			*out << "\n";
	}
	for (int i = 0; i < B->size(); i++) {
		(*B)[i]->ThrowAttack(-1, B, A, (out == NULL ? NULL : out));
		if (out != NULL)
			*out << "\n";
	}

	bool Adead = true;
	for (int i = 0; i < A->size(); i++) {
		if (!(*A)[i]->CheckDead(A, B, out)) {
			Adead = false;
		}
	}

	if (out != NULL)
		*out << "\n";

	bool Bdead = true;
	for (int i = 0; i < B->size(); i++) {
		if (!(*B)[i]->CheckDead(B, A, out)) {
			Bdead = false;
		}
	}

	if (out != NULL)
		out->flush();

	bool Agregate = !(Adead + Bdead);
	return Agregate;
}

void Battle::AutoBattle(std::ostream* out){
	while (Turn(out));
}
