# GameInventory
Для развертывания приложения используйте команду:
docker-compose up -d

Для применения миграций в БД используйте:

dotnet-ef database --startup-project ..\GameInventory\GameInventory.csproj update -v
for /d /r .. %%d in (bin,obj) do @if exist "%%d" rd /s /q "%%d" 

Реализованные API:

/register 
POST запрос с телом в формате:
{
  name: "",
  email: "",
  password: ""
}
Обратите внимание, что пароль должен иметь строчные и прописные латинские буквы, цифры и знаки

/items 
GET запрос для получения предметов в инвентаре пользователя

/fill/{count}
GET запрос для добавления в инвентарь {count} случайных предметов из файла test_items_ids.txt


