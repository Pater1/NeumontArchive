#pragma once
#include<string>
#include "Character.h"

class Wizard : public Character{
	static std::string defaultNames[];

	public:
		Wizard(std::string nameOverride = "");
		~Wizard();

		virtual Character* clone();

	protected:
		virtual void LevelUp(int level);
};

