#include "Barbarian.h"
#include "RandUtils.h"
#include "SingleEnemyHit.h"
#include "FalloffMultiEnemyHit.h"

std::string Barbarian::defaultNames[] = {
	"Lady Pruatis Chandler, Lightning-Bearer",
	"Unaerris Pegason the Valet",
	"Mad Barmaid, Wandaonna Dagarkin"
};

Barbarian::Barbarian(std::string nameOverride): Character(20){
	if (nameOverride == "") {
		characterName = defaultNames[RandInt(3)];
	} else {
		characterName = nameOverride;
	}
	className = "Barbarian";
	attacks = {
		new SingleEnemyHit(0.4, " swings their sword at "),
		new FalloffMultiEnemyHit(0.3, 0.4, " maddly rushes enemy party.")
	};
}

Barbarian::~Barbarian() {}

Character* Barbarian::clone() {
	Character* ret = new Barbarian();
	Character::ConvenienceBaseClone(this, ret);
	//TODO: copy class-specific character data here
	return ret;
}

void Barbarian::LevelUp(int lvl) {
	maxHP += 10;
	curHP = maxHP;

	if (cachedOut != nullptr)
		*FullName(cachedOut) << " has leveled up! They are now level " << lvl << ", and has " << maxHP << "hp!";
}