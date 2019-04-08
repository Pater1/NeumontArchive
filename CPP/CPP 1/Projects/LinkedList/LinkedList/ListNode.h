#pragma once

template<typename T>
class ListNode{
	public:
		ListNode() {}

		ListNode(T data) {
			dtm = data;
		}

		~ListNode() {}

	protected:
		ListNode<T>* next{nullptr};
		ListNode<T>* previous{nullptr};

		T dtm{0};

	public:
		ListNode<T>* const Next() { return next; }
		ListNode<T>* const Previous() { return previous; }
		void Next(ListNode<T>* nxt) { next = nxt; }
		void Previous(ListNode<T>* prv) { previous = prv; }

		T const& GetData() { return dtm; }
};

