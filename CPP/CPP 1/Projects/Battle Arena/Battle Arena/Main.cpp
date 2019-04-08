#include<iostream>

#include "Character.h"

#include "Cleric.h"
#include "Necromancer.h"
#include "Wizard.h"
#include "Barbarian.h"
#include "Rogue.h"

#include "NamesDatabase.h"

#include "Battle.h"
#include "Party.h"

#include "Arena.h"

#include "Currency.h"

#include "LoginUtils.h"

int main(){
	std::ostream* out = &std::cout;
	Arena* a = new Arena(out , Arena::InitializePlayers(out), Arena::GenerateRivals(2));
	a->Fight(500);
	delete a;
	return 0;
}