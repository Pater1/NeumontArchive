#pragma once

#include<string>

#include"Character.h"

class Cleric : public Character {
	static std::string defaultNames[];

	public:
		Cleric(std::string nameOverride = "");
		~Cleric();

		virtual Character* clone();

	protected:
		virtual void LevelUp(int level);
};