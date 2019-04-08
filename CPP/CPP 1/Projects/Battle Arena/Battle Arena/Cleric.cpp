#include "Cleric.h"
#include "RandUtils.h"
#include "SingleEnemyHit.h"
#include "SelfHeal.h"

std::string Cleric::defaultNames[] = {
	"Holy Blacksmith, Leokul",
	"Lady Victaatis Carter, Cleric of Many Gods",
	"Cleric Gramorn Tarmikos the Undaunted"
};

Cleric::Cleric(std::string nameOverride) : Character(17){
	if (nameOverride == "") {
		characterName = defaultNames[RandInt(3)];
	} else {
		characterName = nameOverride;
	}
	className = "Cleric";
	attacks = {
		new SingleEnemyHit(0.25, " swings their mace at "),
		new SelfHeal(0.5)
	};
}

Cleric::~Cleric(){}

Character* Cleric::clone() {
	Character* ret = new Cleric();
	Character::ConvenienceBaseClone(this, ret);
	//TODO: copy class-specific character data here
	return ret;
}

void Cleric::LevelUp(int lvl) {
	maxHP += 10;
	curHP = maxHP;

	if (cachedOut != nullptr)
		*FullName(cachedOut) << " has leveled up! They are now level " << lvl << ", and has " << maxHP << "hp!";
}
