**.NetPDO** allows you to connect to database in  c# PDO style.

	/*
	* Samples
	*/
	//using Prepared statement Method 1
	NetPDOv2.PDO db = new NetPDOv2.PDO();
	db._connect();
	db.prepare("select * from table WHERE column1 = @c1 and column2 = @c2");
	db.bindValue("@c1", "2");
	db.bindValue("@c2", "Value 2");
	db.execute(true); //execute(bool isFetch=false) : Optional Parameter isFetch must be true to use fetchAllAsStr and fetchAllAsObj;
	string StringResult = db.fetchAllAsStr(); //Get response as JSON string
	JArray JsonResult = db.fetchAllAsObj(); //Gets response as JSON
	//using Prepared statement Method 2
	NetPDOv2.PDO db2 = new NetPDOv2.PDO();
	db._connect();
	db.prepare("select * from table WHERE column1 = ? AND column2 = ?");
	string[] data = { "2", "Value 2" };
	db.bindValues(data);
	db.execute(true); //execute(bool isFetch=false) : Optional Parameter isFetch must be true to use fetchAllAsStr and fetchAllAsObj;
	string StringResult2 = db.fetchAllAsStr(); //Get response as JSON string
	JArray JsonResult2 = db.fetchAllAsObj(); //Gets response as JSON
	/*
	* DBTransaction Class allows you to do a quick DB transaction
	*/
	// Check if row exists usage isExist(query, parameter)
	NetPDOv2.dbTransactions dbtrans = new NetPDOv2.dbTransactions();
	if (dbtrans.isExist("SELECT id FROM table WHERE condition = true", new string[]{""})) {
		//Row Exists, do something
	} 
	//Selects Rows from DB. Usage selectFromQuery(query, parameters, isObj)
	NetPDOv2.dbTransactions.queryResult result = dbtrans.selectFromQuery("SELECT * FROM table WHERE column = ?", new string[] { "value" }, true); //If Query is successful, result.resultAsObject will return have the result