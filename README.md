# Приложение ООО "Посуда"

Приложение ООО "Посуда" представляет собой **WPF-приложение** с базой данных **SQL Server**, написанное на языке программирования C#, которое является маркетплейсом посуды. В приложении предусмотрена система авторизации и входа в систему _"как Гость"_. В приложении есть три каталога с функциями сортировки и фильтров ради поиска нужных товаров для трех типов пользователей: **Администратор** (`CatalogAdministrator`), **Менеджер/Клиент** (`CatalogAuthorized`) и **Гость** (`CatalogNonAuthorized`). В приложении также есть система добавления (`ProductAddForm`), редактирования (`ProductRedactForm`) и удаления товаров (может выполнять только Администратор). Администратор, Менеджер, Клиент и Гость могут просматривать каталог товаров. Когда в каталоге нажата специальная кнопка для заказа, товар автоматически добавляется в корзину (`BasketForm`) для дальнейшего оформления заказа.

## Начало работы

Эти инструкции помогут вам получить копию проекта и запустить его на вашем локальном компьютере для разработки и тестирования.

### Необходимые условия

Для установки программного обеспечения вам потребуется:

* **Visual Studio 2022**
* **SQL Express 2022**
* **SQL Server Management Studio** (SSMS)

### Установка

Пошаговая инструкция по установке:

1. **Клонирование репозитория**

   Клонируйте репозиторий с помощью следующей команды:

   ```bash
   git clone https://github.com/silly-person-on-the-cheese-planet/DishesApplication.git
   ```

   Вы также можете клонировать репозиторий через интерфейс Visual Studio. Для этого откройте Visual Studio, выберите _"Clone a repository"_ и введите URL репозитория .

2. **Восстановление базы данных**

   Запустите **SQL Server Management Server** и восстановите базу данных с помощью файла `My.bak` в каталоге DataBase-bak-file внутри проекта.

3. **Редактирование строки подключения**

   Откройте файл `App.config` и отредактируйте строку подключения к базе данных. Пример строки подключения:

   ```xml
   <connectionStrings>
	<add name="MyDB"
		 connectionString="Server=ВАШ-СЕРВЕР;Database=My;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;"
		 providerName="System.Data.SqlClient" />
   </connectionStrings>
   ```

4. **Запуск проекта**

   Откройте проект в Visual Studio и запустите его.

## Автор

* **Тараканов Павел** - *Initial work* - [silly-person-on-the-cheese-planet](https://github.com/silly-person-on-the-cheese-planet)
