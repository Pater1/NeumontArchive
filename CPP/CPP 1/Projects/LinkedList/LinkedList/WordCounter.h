#pragma once
#include "LinkedList.h"
#include "CountedWord.h"
#include <iostream>
#include <sstream>
#include <memory>
#include <regex>
#include <algorithm>

class WordCounter : public LinkedList<std::shared_ptr<CountedWord>>{
	public:
		WordCounter(std::istream& os) : LinkedList(){
			ParseInStream(os);
		}
		~WordCounter() {

		}

		void ParseInStream(std::istream& os) {
			std::string word;
			while (os >> word) {
				ParseWord(word);
			}
		}

		void ParseWord(std::string word) {
			const std::regex wordReg("([A-Za-z]+)");
			const std::regex notWordReg("([^A-Za-z]+)");

			std::string wr = std::regex_replace(word, wordReg, "");
			std::string pn = std::regex_replace(word, notWordReg, "");

			std::transform(wr.begin(), wr.end(), wr.begin(), ::tolower);
			std::transform(pn.begin(), pn.end(), pn.begin(), ::tolower);

			AddWord(wr);
			AddWord(pn);
		}

		void AddWord(std::string word) {
			ListNode<std::shared_ptr<CountedWord>>* current = this;
			for (int j = 0; j < NodeCount(); j++) {
				current = current->Next();
				if (current == this) break;
				if (current->GetData()->AddWord(word)) return;
			}

			std::shared_ptr<CountedWord> nw(new CountedWord(word));
			AddToTail(nw);
		}

	public:
		std::ostream& Print(std::ostream& os) {
			ListNode<std::shared_ptr<CountedWord>>* current = this;
			for (int j = 0; j < NodeCount(); j++) {
				current = current->Next();
				os << *(current->GetData()) << "\n";
				if (current == this) break;
			}
			return os;
		}
};

inline std::ostream& operator <<(std::ostream& os, WordCounter& obj) {
	return obj.Print(os);
}