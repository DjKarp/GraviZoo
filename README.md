<h1 align="center">Gravi Zoo — Mini Game Prototype</h1>

<p align="center"><b>Прототип мини-игры по техническому заданию: фигуры падают с эффектом песка, игрок собирает три одинаковых — чтобы очистить поле.</b></p>

## 🧰 Платформа
Unity (Android/iOS)

Zenject (DI + Pooling)

MVP-архитектура

Без меню, сейвов и сложной графики (прототип)

## ✨ Скриншоты
<p align="center">
<img src="https://redleggames.com/Games/GraviZoo/GraviZoo_Screen_01.png" width="200"/>
<img src="https://redleggames.com/Games/GraviZoo/GraviZoo_Screen_02.png" width="200"/>
<img src="https://redleggames.com/Games/GraviZoo/GraviZoo_Screen_03.png" width="200"/>
<img src="https://redleggames.com/Games/GraviZoo/GraviZoo_Screen_04.png" width="200"/>
</p>
    
## 🎮 Геймплей
Игровое поле заполняется случайными тайлами (Tile).

Каждый тайл — комбинация: форма + цвет рамки + животное.

Количество экземпляров каждого тайла кратно 3.

При старте — тайлы сыпятся сверху с физикой (гравитация, столкновения).

Игрок нажимает на тайл → он улетает в action-бар.

В action-бар'е:

Если собрано 3 одинаковых — они исчезают.

Если заполнено 7 ячеек — проигрыш.

Цель — очистить поле.


## ⚙️ Особенности реализации
#Zenject:

DI (Presenter/View separation)

Object Pool для Tile

Factory для создания тайлов из TileModel

#MVP:

GameView реализует IGameView, IMovedTile, ITileFieldService

GamePresenter управляет логикой (без зависимости от Unity)

#TileSpawner:

Управляет генерацией, сбросом и удалением тайлов на поле


## 📁 Структура проекта
<pre> ```_Project/
├── GameData/              # Config Scriptable Object игры.
├── Prefabs/               # Shape (разные фигуры) и Tile (стандартный Tile и Tileas с эффектами) префабы. 
├── Scenes/                # Все сцены игры 
│   ├── Bootstrap          # Разгоночная сцена, содержит только загрузочный экран, с неё запускаются все остальные сцены
│   └── Gameplay           # Сцена с геймплеем
├── Scripts/
│   ├── Core/              # Bootstrap и EntryPoint скрипты
│   ├── Data/              # GameConfig Scriptable Object
│   ├── Installers/        # Zenject Installer'ы
│   ├── Model/             # Модели компонентов игры
│   ├── Presenter/         # GamePresenter скрипт, соединяющий между собой View и Model
│   ├── Signals/           # СZenject сигналы для реагирования на тестовый UI, для примера
│   ├── Tile/              # Tile, Данные и потомки от Tile
├── └── View/              # Интерфейсы и скрипты относящиеся к View
├── Settings/              # URP настройки и Физические данные
└── Textures/              # Спрайты голов животных, эффектов тайлов и UI. Sprite Atlas.
``` </pre>
---

## Scriptable Object
Содержит настройки для кастомизации игры:

| Поле                     | Описание                                                                                                                                          |
|--------------------------|---------------------------------------------------------------------------------------------------------------------------------------------------|
| `MaxCountTiles`          | Максимальное количество тайлов, которые сыпяться из точки спавна (облако) в стакан (игровое поле). В референсе - 63.                              |
| `MatchCountTiles`        | Количество тайлов в экшен баре, которое требуется для их исчезновения. По другому - это количество одинаковых тайлов. По заданию - 3.             |
| `TimeSpawn`              | Интервал с которым падают тайлы.                                                                                                                  |
| `MoveTileTime`           | Время для анимации полёта тайла из места, где по нему щёлкнули, до экшен бара.                                                                    |
| `NumberTilesToUnfreeze`  | Количество тайлов, которое нужно отправить в облако, чтобы разморозились Freezed тайлы.                                                           |
| `TileEffectCount`        | Список из названия эффекта особых тайлов - TileEffect и количества их на уровне. Т.е. сколько и каких специальных тайлов будет создаваться.       |
| `TilePrefab`             | Префаб стандартного тайла                                                                                                                         |

<img src="https://redleggames.com/Games/GraviZoo/GraviZoo_GameConfig.png"/>


## Как запустить

Скачай архив, распакуй и запусти готовый Build:
https://github.com/DjKarp/GraviZoo/releases

скачать с Google Drive -> 
https://drive.google.com/file/d/1JDhBCWNzHYcS1IkfUxfhLWcgkudq22vV/view?usp=sharing


Склонируй проект:

git clone https://github.com/DjKarp/GraviZoo.git

Открыть в Unity 2022.3+ (URP)

Сцена запуска: Bootstrap, Gameplay

Играй! 🎉
