#pragma once
#include "Character.h"
class PlayerWrapper : public Character{
	public:
		PlayerWrapper(Character* cha);
		~PlayerWrapper();

	private:
		Character* trueCharacter;

	public:
		virtual bool ThrowAttack(int atkIndex, std::vector<Character*>* allys, std::vector<Character*>* targets, std::ostream* out = NULL);
		virtual std::ostream* FullName(std::ostream* out);

		virtual int AlterHP(double HPDelta, std::ostream* out = NULL);
		virtual int AlterHP(int delta, std::ostream* out = NULL);

		virtual bool CheckDead(std::vector<Character*>* allys, std::vector<Character*>* targets, std::ostream* out = NULL);
		virtual bool PeekDead();
		virtual int PeekHP();

		virtual Currency& GetMoney();

		virtual int GetEditWins(int delta);
		virtual int GetEditLosses(int delta);

		virtual Character* clone();

		virtual double XPDrop();
		virtual void XPPickup(double xp);

		std::ostream* cachedOut{ NULL };

		virtual void Revive(std::vector<Character*>* allys, std::vector<Character*>* targets, std::ostream* out);

	protected:
		virtual void LevelUp(int level);
};

