#pragma once

#include<string>
#include<vector>

#include"Currency.h"

class Attack;

class Character {
	public:
		static void ConvenienceBaseClone(Character* from, Character* to);

	protected:
		Character(int HP_max);

	public:
		~Character();

	public:
		bool hidden{ 0 };


	public:
		std::string className{ "Generic Farmer" };
		std::string characterName{"Herp McDerpson"};

		int maxHP;
		int curHP;
		int TurnDelta_HP{ 0 };

		int wins{ 0 };
		int losses{ 0 };

		double curXP{ 0 };
		double xpToNextLevel{ 1 };
		int level{ 1 };
		
		std::vector<Attack*> attacks;
		Currency* money;

	public:
		std::ostream* cachedOut{ NULL };

	public:
		virtual bool ThrowAttack(int atkIndex, std::vector<Character*>* allys, std::vector<Character*>* targets, std::ostream* out = NULL);
		virtual std::ostream* FullName(std::ostream* out);

		virtual int AlterHP(double HPDelta, std::ostream* out = NULL);
		virtual int AlterHP(int delta, std::ostream* out = NULL);

		virtual bool CheckDead(std::vector<Character*>* allys, std::vector<Character*>* targets, std::ostream* out = NULL);
		virtual bool PeekDead();
		virtual int PeekHP();

		virtual Character* clone() = 0;

		virtual double XPDrop();
		virtual void XPPickup(double xp);

		virtual Currency& GetMoney();

		virtual int GetEditWins(int delta);
		virtual int GetEditLosses(int delta);

		virtual void Revive(std::vector<Character*>* allys, std::vector<Character*>* targets, std::ostream* out);

		virtual void LevelUp(int level) = 0;

		Character* DeleteProtect(Character* from);
};