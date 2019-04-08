#pragma once
#include "Character.h"
class Necromancer : public Character{
	static std::string defaultNames[];

	public:
		Necromancer(std::string nameOverride = "");
		~Necromancer();

		virtual Character* clone();

	protected:
		virtual void LevelUp(int level);
};

