#include "SimpleUndead.h"
#include "RandUtils.h"
#include "SingleEnemyHit.h"
#include <algorithm>

#include "NamesDatabase.h"

std::string SimpleUndead::sudoClasses[] = {
	"Draugr",
	"Ghoul",
	"Jiangshi",
	"Revenant",
	"Skeleton",
	"Wight",
	"Zombie",
	"Banshee",
	"Ghost",
	"Spectre",
	"Phantom",
	"Poltergeist",
	"Shadow",
	"Wraith"
};

SimpleUndead::SimpleUndead():Character(1){
	className = sudoClasses[RandInt(14)];
	characterName = NamesDatabase::AssembleRandomFullName();
	attacks = {
		new SingleEnemyHit(0.00001, " punches "),
	};
}


SimpleUndead::~SimpleUndead(){

}

bool SimpleUndead::CheckDead(std::vector<Character*>* allys, std::vector<Character*>* targets, std::ostream* out) {
	bool b = Character::CheckDead(allys, targets, out);

	if (b) {
		allys->erase(std::remove(allys->begin(), allys->end(), this), allys->end());
		delete this;
	}

	return b;
}

Character* SimpleUndead::clone() {
	Character* ret = new SimpleUndead();
	Character::ConvenienceBaseClone(this, ret);
	//TODO: copy class-specific character data here
	return ret;
}

void SimpleUndead::LevelUp(int ) {
	maxHP ++;
	curHP = maxHP;
}

void SimpleUndead::Revive(std::vector<Character*>* allys, std::vector<Character*>* targets, std::ostream* out) {
	curHP = -5;
	CheckDead(allys, targets, out);
}