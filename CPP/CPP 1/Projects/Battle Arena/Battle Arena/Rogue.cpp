#include "Rogue.h"
#include "RandUtils.h"
#include "SingleEnemyHit.h"
#include "Hide.h"

std::string Rogue::defaultNames[] = {
	"Pursediver, Pruatra Shipsail",
	"Yllaerris Trickfoot the Wheeler",
	"Nerisella Huntinghawk, Silent Tyrantfeller"
};

Rogue::Rogue(std::string nameOverride): Character(10){
	if (nameOverride == "") {
		characterName = defaultNames[RandInt(3)];
	}
	else {
		characterName = nameOverride;
	}
	className = "Rogue";
	attacks = {
		new SingleEnemyHit(0.2, " thrusts their dagger into ", 0.9, 0.1),
		new Hide(0.75, " sneaking in the shadows.")
	};
}

void Rogue::LevelUp(int lvl) {
	maxHP += 10;
	curHP = maxHP;

	if (cachedOut != nullptr)
		*FullName(cachedOut) << " has leveled up! They are now level " << lvl << ", and has " << maxHP << "hp!";
}


Rogue::~Rogue(){}


Character* Rogue::clone() {
	Character* ret = new Rogue();
	Character::ConvenienceBaseClone(this, ret);
	//TODO: copy class-specific character data here
	return ret;
}