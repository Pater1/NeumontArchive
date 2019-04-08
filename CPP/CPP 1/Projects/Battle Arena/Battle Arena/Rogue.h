#pragma once
#include "Character.h"
class Rogue : public Character{
	static std::string defaultNames[];

	public:
		Rogue(std::string nameOverride = "");
		~Rogue();

		virtual Character* clone();

	protected:
		virtual void LevelUp(int level);
};

