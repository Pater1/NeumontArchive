#pragma once
#include "ListNode.h"
#include<iterator>

template<typename T>
class LinkedList : public ListNode<T>{
	protected:
		LinkedList() {
			this->Next(this);
			this->Previous(this);
		}
	public:
		LinkedList(T init) : LinkedList(){
			AddToHead(init);
		}
		LinkedList(T* init, int num) : LinkedList(){
			for (int i = 0; i < num; ++i) {
				AddToHead(init[i]);
			}
		}
		//LinkedList(T init, T... cont) : LinkedList() {
		//	AddToHead(init, cont);
		//}
		~LinkedList() {
			while (NodeCount() > 0)
				RemoveByIndex(NodeCount()-1);
		}

	//next = head; previous = tail

	private:
		int nodeCount{ 0 };

		ListNode<T>* GetNodeByIndex(int i) {
			ListNode<T>* current = Next();
			for (int j = 0; j < i; j++) {
				current = current->Next();
			}
			return current;
		}

	public:
		T GetValueByIndex(int i){
			return GetNodeByIndex(i)->GetData();
		}

		T GetValue(T val){
			ListNode<T>* current = Next();
			for (int j = 0; j < NodeCount(); j++) {
				current = current->Next();
				if (current->GetData() == val) break;
			}
			return current->GetData();
		}
		
		int GetIndex(T val){
			ListNode<T>* current = Next();
			for (int j = 0; j < NodeCount(); j++) {
				current = current->Next();
				if (current->GetData() == val) return j;
			}
			return -1;
		}
	
		bool RemoveByIndex(int i){
			if (i < 0 || i >= NodeCount()) return false;

			ListNode<T>* current = GetNodeByIndex(i);
			ListNode<T>* prev = current->Previous();
			ListNode<T>* nxt = current->Next();

			prev->Next(nxt);
			nxt->Previous(prev);

			nodeCount--;

			delete current;

			return true;
		}
	
		bool RemoveFirstValue(T val){
			int i = GetIndex(val);
			if(i < 0) return false;

			RemoveByIndex(i);
			return true;
		}
	
		void AddToHead(T val){
			ListNode<T>* node = new ListNode<T>(val);
			ListNode<T>* nxt = this->Next();

			node->Previous(this);
			node->Next(nxt);

			nxt->Previous(node);
			this->Next(node);

			nodeCount++;
		}
	
		void AddToTail(T val){
			ListNode<T>* node = new ListNode<T>(val);
			ListNode<T>* prev = this->Previous();

			node->Next(this);
			node->Previous(prev);

			prev->Next(node);
			this->Previous(node);

			nodeCount++;
		}

		/*void AddToHead(T val, T... cont){
			AddToHead(val);
			AddToHead(cont);
		}
		
		void AddToTail(T val, T cont ...){
			AddToTail(val);
			AddToTail(cont);
		}*/

		//bool InsertAtIndex(int i) {
		//	return false;
		//}

		//bool InsertAfter(T val){
		//	return false;
		//}

		int const NodeCount() { return nodeCount; };
};