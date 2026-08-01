# kaguyahouraisan.com
 Для работы, требуется PostgreSQL
 В файле appsettings.Development.json отредактируйте cтроку подключения ConnectionString под себя
```
"ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=5432;Database=НАЗВАНИЕ_БАЗЫ_ДАННЫХ;UserId=ПОЛЬЗОВАТЕКЛЬ;Password=ПРИДУМАЙТЕ_СВОЙ"
  },
```
 
 Чтобы попасть на страницу админки, перейдите по адресу AdminkaMorkovka/Index

 Пароль: admindevelopment

 Для смены пароля админки:
 1) Создайте новое консольное приложение C# и добавьте пакет Microsoft.AspNetCore.Identity
 2) Вставьте следующий код:
 ```
using Microsoft.AspNetCore.Identity;

var hasher = new PasswordHasher<string>();

Console.Write("Введите пароль: ");
string password = Console.ReadLine()!;

string hash = hasher.HashPassword(null, password);

Console.WriteLine();
Console.WriteLine("Хэш:");
Console.WriteLine(hash);

Console.WriteLine();
Console.WriteLine("Полученный хеш скопируйте в appsettings.Development.json...");
Console.ReadKey();
```
3) Полученный хеш скопируйте в appsettings.Development.json в строку AdminPasswordHash
```
"AdminPasswordHash": "Вставить полученный хеш.",
```

 Copyright (c) 2026 wimirei
 https://kaguyahouraisan.com
