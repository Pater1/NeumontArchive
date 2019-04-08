#include "Necromancer.h"
#include "RandUtils.h"
#include "SumonSimpleUndead.h"
#include "UniformMultiLifeDrain.h"


std::string Necromancer::defaultNames[] = {
	"Fallen Monk, Pettumal Duskwalker",
	"Corpsemaker Brekhar",
	"Amuxir Atheir, The Rotting"
};

Necromancer::Necromancer(std::string nameOverride) : Character(12){
	if (nameOverride == "") {
		characterName = defaultNames[RandInt(3)];
	}
	else {
		characterName = nameOverride;
	}
	className = "Necromancer";
	attacks = {
		new SumonSimpleUndead(),
		new UniformMultiLifeDrain(0.0001, 0.5)
	};
}


Necromancer::~Necromancer() {}

Character* Necromancer::clone() {
	Character* ret = new Necromancer();
	Character::ConvenienceBaseClone(this, ret);
	//TODO: copy class-specific character data here
	return ret;
}

void Necromancer::LevelUp(int ) {
	maxHP += 10;
	curHP = maxHP;

	if (cachedOut != nullptr)
		*FullName(cachedOut) << " has leveled up! They are now level " << level << ", and has " << maxHP << "hp!";
}