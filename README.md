# Awesome Files Archive Service

## Описание проекта

> Программа для получения информации о файлах встроенного в проект хранилища AwesomeStorage, создания задач на упаковку этих файлов в архив и скачивания этих архивов. Выполнена в виде backend-приложения и консольного приложения.

## Требования

Для установки и запуска проекта необходим .NET 9.0

## Установка

```sh
git clone https://github.com/psengx/TestTaskKaspersky/
```

## Структура проекта

```
├── AwesomeFilesCore
│   ├── Models
│   │   ├── ArchiveTask.cs // Модель задачи архивации
│   └── Services
│       ├── ArchiveService.cs // Сервис для работы с задачами архивации
│       └── FileService.cs // Сервис для работы с файлами хранилища
├── AwesomeStorage // Встроенное хранилище файлов
│   ├── file.png
│   ├── file.svg
│   └── file.txt
├── ClientConsoleApp // Консольное приложение 
│   ├── BackendClient.cs // Клиент для работы с сервисами (отправляет запросы в сервисы)
│   ├── CommandParser.cs // Обработчик команд, введеных в консоль (отправляет запросы в клиент)
│   └── Program.cs // Основный цикл программы
├── AwesomeWebApi // Backend-приложение
│   ├── Controllers
│   │   ├── ArchiveController.cs // Контроллер для работы с сервисом архивации
│   │   └── FilesController.cs // Контроллер для работы с сервисом файлов
│   ├── Program.cs // Входная точка в программу
└── UnitTests // Немного тестов
    └── ArchiveTests.cs
```

## Описание AwesomeWebApi

> Backend-приложение для работы с хранилищем AwesomeStorage.

## Запуск WebAPI

Откройте директорию проекта AwesomeWebApi в консоли и используйте команду для запуска:

```sh
dotnet run
```

### Эндпоинты

#### GET /api/files

Получение всех файлов из хранилища AwesomeStorage

Формат ответа:

```json
[
  "file.png",
  "file.svg",
  "file.txt"
]
```

---

#### PUT /initialize 

Инициализация задачи архивации выбранных файлов

Запрос: список имён файлов в формате json

```json
[
  "file.png",
  "file.svg",
  "file.txt"
]
```

Ответ: идентификатор инициализированной задачи

```json
"46f866ec-0705-4251-934a-94f9c3cdba95"
```

---

#### GET /status

#### Параметры

- id - идентификатор задачи архивации, полученный при инициализации, в формате uuid:

    `46f866ec-0705-4251-934a-94f9c3cdba95`

#### Формат ответа

Ответ приходит в виде статуса задачи архивации.

- Pending - запрос на архивацию отправлен
- Processing - в процессе архивации
- Done - архивация завершена

- Failed - ошибка выполнения запроса

    [Error message] - сообщение об ошибке

---

#### GET /download

#### Параметры

- id - идентификатор задачи архивации, полученный при инициализации, в формате uuid:

    `46f866ec-0705-4251-934a-94f9c3cdba95`

#### Формат ответа
Ответом на запрос является файл - архив, найденный по указанному идентификатору
В случае если:

- Архив не готов:

        Http Status: 202
        Archiving is not done

- Возникла ошибка при выполнении архивации:

        Http Status: 400
        Failed
        [Error message] - сообщение об ошибке

- Задача с таким идентификатором не найдена:

        Http Status: 404
        Task not found

## Описание консольного приложения

> Консольное приложение для работы с хранилищем AwesomeStorage.

## Запуск Client

Откройте директорию проекта ClientConsoleApp в консоли и используйте команду для запуска:

```sh
dotnet run
```

### Команды

#### list

Получение всех файлов из хранилища AwesomeStorage

Формат ответа:
```sh
file.png file.svg file.txt
```

Возможная ошибка:

- Кроме команды написано что-то лишнее, например, `list files`:

        Wrong command (maybe you wrote more than just command)

---

#### create-archive 

Аргументы:
- названия файлов через пробел

Пример использования:

```sh
create-archive file.txt file.svg
```

Формат ответа:

```sh
Task initialized. Your task id: d50547ef-03af-4674-ac74-c2c814c78e59
```

Возможная ошибка:

- Названия файлов не введены:

        Wrong command: enter file names

---

#### status

Аргументы:
- идентификатор задачи

Пример использования:

```sh
status 3aa76bf4-a6d4-41fc-9660-ec2ae2ad0e3e
```

#### Формат ответа

Ответ приходит в виде статуса задачи архивации.

- Pending - запрос на архивацию отправлен
- Processing - в процессе архивации
- Done - архивация завершена
- Failed - ошибка выполнения запроса

    [Error message] - сообщение об ошибке

Возможные ошибки:

- Введено больше 1 аргумента или не введено ни одного:

        Wrong command: command 'status' has 1 argument - Task ID

- Аргумент введен неверно (не GUID):

        Argument must be Guid

- Задача архивации не найдена:

        Task not found 

---

#### download

Аргументы:
- идентификатор задачи
- путь для сохранения архива

Пример использования:

```sh
download 3aa76bf4-a6d4-41fc-9660-ec2ae2ad0e3e E://kasp
```

#### Формат ответа

```sh
Task downloaded to E://kasp
```

Возможные ошибки:

- Введено больше 1 аргумента или не введено ни одного:

        Wrong command: command 'download' has 2 arguments - Task ID and Download Path

- Аргумент для идентификатора введен неверно (не GUID):

        Argument must be Guid

- Задача архивации не найдена:

        Task not found

- Введен неверный путь:

        Enter correct path
