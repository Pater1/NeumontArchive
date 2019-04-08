#pragma once
#include "Character.h"
class Barbarian : public Character{
	static std::string defaultNames[];

	public:
		Barbarian(std::string nameOverride = "");
		~Barbarian();

		virtual Character* clone();

	protected:
		virtual void LevelUp(int level);
};

