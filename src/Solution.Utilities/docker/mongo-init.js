use nexuscore

db.createUser(
	{
		user: "nexuscore",
		pwd: "UPjhg?eLKhAs9?:*jR%Ta?-zz",

		customData: {},

		roles: [{ "role": "readWrite", "db": "nexuscore" }, { "role": "dbAdmin", "db": "nexuscore" }],
	}
)