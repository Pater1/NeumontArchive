#include "Wizard.h"
#include<string>
#include "RandUtils.h"

#include "SingleEnemyHit.h"
#include "UniformMultiEnemyHit.h"

std::string Wizard::defaultNames[] = {
	"Arkane Baker, Ravaatris",
	"Lady Yllaora Huntinghawk the Powerful",
	"Lord Oloneiros Cupshigh the Merciless"
};

Wizard::Wizard(std::string nameOverride) : Character(14) {
	if (nameOverride == "") {
		characterName = defaultNames[RandInt(3)];
	}
	else {
		characterName = nameOverride;
	}
	className = "Wizard";
	attacks = {
		new SingleEnemyHit(0.5, " shoots a magic missle at "),
		new UniformMultiEnemyHit(0.25, " rains acid on the enemy party.\n")
	};
}


Wizard::~Wizard(){

}

Character* Wizard::clone() {
	Character* ret = new Wizard();
	Character::ConvenienceBaseClone(this, ret);
	//TODO: copy class-specific character data here
	return ret;
}

void Wizard::LevelUp(int lvl) {
	maxHP += 10;
	curHP = maxHP;

	if (cachedOut != NULL)
		*FullName(cachedOut) << " has leveled up! They are now level " << lvl << ", and has " << maxHP << "hp!";
}