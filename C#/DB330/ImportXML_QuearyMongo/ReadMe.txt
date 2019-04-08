Readme-
	Setup-
		All areas where values must be changed in code are labeled with the following comment schema:
			//!!![Basic info about what to update, or if to ignore]
		Note the three exclamation points preceeding the details. These are to provide easy search with Visual Studio's built-in Ctrl+F function.
		
		There are two projects, each their own separate console applications.
			ImportXML-
				Reads in the provided XML, and parses it into the database.
				Side note: there is no implementation-specific code here. In theory, this can be used with any valid XML and any MongoDB instance to import the data into.
				Side-side note: This means that there is no imposed schema any more than that which is prescribed by the XML itself. This *may* have ended up backfiring a little...
				
			QuearyMongo-
				Run async quearies as prescribed in the assignment. Due to the aformentioned side-side note, these quearies are far from elegant, and can take quite a while to execute on the first operation.
				However! All returns are actively cached (through a system that could use some refactoring, admitedly), thus making subsequent searches of these of similar quearies sub-millisecond*
					*Just a guess based on how fast results show up. No actual measurement of speed has been done.
				Caches are wiped and requearied if ever either
					1) the cache collection fails to return a sutable cache object, or
					2) the developer explicitly passes 'true' into the optional 'forceRecalc' parameter (which defaults to 'false')
				Seeing as you guys aren't using my Mongo instance, expect queary 0 to be pretty slow, but please run the program a second time to apprechiate the improvement provided by the cache.
				
Schema (Displayed in sudo-json)-
	Customer:
		{
			_id: [objectIdValue]
			customer: {
				name: [nameValue]
				email: [emailValue]
				customerId: [customerIdValue]
				orders: {
					order: [
						{
							orderId: [orderIdValue]
							customerId: [customerIdValue]
							orderTotal: [orderTotalValue]
							lines: {
								line: [
									{
										orderLineId: [orderLineIdValue]
										productId: [productIdValue]
										price: [pricePerUnitValue]
										qty: [quantityPurchasedValue]
										lineTotal: [lineTotalValue]
									}
									...
								]
							}
						}
						...
					]
				}
			}
		}
		
	Cache:
		{
			_id: [objectIdValue]
			metadata:{
				tag: [sudoKeyUsedToQuearyCacheBackOutOfDB]
				(insert further metadata here as needed)
			}
			values: [
				(all the data from the serialized object, usually a collection)
			]
		}
		
		
		
		
		
		
		
		
		
		