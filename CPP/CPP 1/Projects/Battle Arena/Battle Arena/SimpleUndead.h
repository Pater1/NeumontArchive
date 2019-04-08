#pragma once
#include "Character.h"
class SimpleUndead : public Character{
	static std::string sudoClasses[];

	public:
		SimpleUndead();
		~SimpleUndead();
		bool CheckDead(std::vector<Character*>* allys, std::vector<Character*>* targets, std::ostream* out);

		virtual Character* clone();

		virtual void Revive(std::vector<Character*>* allys, std::vector<Character*>* targets, std::ostream* out);

	protected:
		virtual void LevelUp(int level);
};

