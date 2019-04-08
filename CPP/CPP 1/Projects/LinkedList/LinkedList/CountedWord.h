#pragma once
#include<string>
#include<iostream>

class CountedWord{
	public:
		CountedWord(std::string word) {
			wrd = word;
		}
		~CountedWord() {}

		bool AddWord(std::string word) {
			bool ret = (wrd == word);
			if (ret) {
				count++;
			}
			return ret;
		}

		int const Count() { return count; }
		std::string const Word() { return wrd; }

	private:
		std::string wrd{ "" };
		int count{ 1 };

	public:
		std::ostream& Print(std::ostream& os) {
			return (os << wrd << " [" << count << "]");
		}
};

inline std::ostream& operator <<(std::ostream& os, CountedWord& obj){
	return obj.Print(os);
}