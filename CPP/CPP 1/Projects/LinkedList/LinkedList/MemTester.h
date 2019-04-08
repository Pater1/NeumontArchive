#pragma once
#include <iostream>
#include <cstddef>
#include <new>

void PrintAllocations(); // pre-declare

namespace
	{
	std::size_t BytesAllocatedRaw = 0;
	std::size_t BytesAllocatedArray = 0;
	std::size_t BytesFreedRaw = 0;
	std::size_t BytesFreedArray = 0;

	struct MemRecord
		{
		void *p;
		std::size_t n;
		struct MemRecord* next;
		};
	typedef struct MemRecord MemRecord_t;

	MemRecord_t * ___MemList = NULL;

	void ___pushMemRecord(void*p, std::size_t n)
		{
		MemRecord_t * newRecord = (MemRecord_t*)malloc(sizeof(MemRecord_t));
		newRecord->p = p;
		newRecord->n = n;
		newRecord->next = NULL;

		if (___MemList == NULL) {
			___MemList = newRecord;
			}
		else
			{
			MemRecord_t * current = ___MemList;
			while (current->next != NULL)
				{
				current = current->next;
				}
			current->next = newRecord;
			}
		}

	std::size_t ___popMemRecord(void*p)
		{
		size_t val = 0;
		MemRecord_t * current = ___MemList;
		MemRecord_t * toAxe = NULL;
		if (___MemList->p == p)
			{
			val = ___MemList->n;
			___MemList = ___MemList->next;
			free(current);
			}
		else
			{
			while (current->next != NULL && current->next->p != p)
				{
				current = current->next;
				}
			if (current->next->p == p)
				{
				toAxe = current->next;
				val = toAxe->n;
				current->next = current->next->next;
				free(toAxe);
				}
			else
				{
				std::cerr << "Error - did not find a record in the mem list to match address " << p << std::endl;
				val = 0;
				}
			}
		return val;
		}

	class MemTester
		{
		public:
			MemTester() { }
			~MemTester() { PrintAllocations(); }
			static const MemTester memTester;
		};
	const MemTester MemTester::memTester;
	}

void * operator new(std::size_t n) //throw(std::bad_alloc)
	{
	void *p = malloc(n);
	if (!p)
		{
		throw std::bad_alloc();
		}
	BytesAllocatedRaw += n;
	___pushMemRecord(p, n);
	return p;
	}

void * operator new[](std::size_t n) //throw(std::bad_alloc)
	{
	void *p = malloc(n);
	if (!p)
		{
		throw std::bad_alloc();
		}
	BytesAllocatedArray += n;
	___pushMemRecord(p, n);
	return p;
	}

	void operator delete(void * p) throw()
		{
		std::size_t n = ___popMemRecord(p);
		free(p);
		BytesFreedRaw += n;
		}

	void operator delete[](void * p) throw()
		{
		std::size_t n = ___popMemRecord(p);
		free(p);
		BytesFreedArray += n;
		}

		void PrintAllocations()
			{
			std::cout << "----------------------------------\n";
			std::cout << "Allocated " << BytesAllocatedRaw << " bytes" << std::endl
				<< "Allocated " << BytesAllocatedArray << " array bytes" << std::endl
				<< "Freed " << BytesFreedRaw << " bytes" << std::endl
				<< "Freed " << BytesFreedArray << " array bytes" << std::endl;
			if (BytesFreedRaw == BytesAllocatedRaw && BytesFreedArray == BytesAllocatedArray)
				{
				std::cout << "ALL HEAP MEMORY FREED" << std::endl;
				}
			else
				{
				std::cout << "\n     YOU ARE LEAKING MEMORY!\n" << std::endl;
				}
			std::cout << "----------------------------------\n";
			}
