#pragma once
#include "Character.h"
class LoginUtils{
	public:
		static Character * LoginAndAssembleCharacter(std::ostream * cacheOut);

	private:
		static int Login();
		static Character* BuildPlayer();

	private:
		LoginUtils() {}
		~LoginUtils() {}
};

