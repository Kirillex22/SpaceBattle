@Turn
Функция: Поворот вокруг собственной оси

Сценарий: Игровой объект может повернуться вокруг собственной оси
Дано космический корабль находится под углом к горизонту в 45 градусов
И имеет угловую скорость 90 градусов
Когда происходит поворот вокруг собственной оси
Тогда космический корабль оказывается под углом 135 градусов к горизонту

Сценарий: Если невозможно определить угла наклона корабля к горизонту, то поворот невозможен
Дано космический корабль, угол наклона к горизонту которого невозможно определить
И имеет угловую скорость 90 градусов
Когда происходит поворот вокруг собственной оси
Тогда возникает ошибка Exception 

Сценарий: Если невозможно определить угловую скорость корабля, то поворот невозможен
Дано космический корабль находится под углом к горизонту в 45 градусов
И угловую скорость корабля определить неозможно
Когда происходит поворот вокруг собственной оси
Тогда возникает ошибка Exception 

Сценарий: Если невозможно изменить угол наклона корабля к горизонту, то поворот невозможен
Дано космический корабль находится под углом к горизонту в 45 градусов
И угловую скорость корабля определить неозможно
И изменить угол наклона к горизонту невозможно
Когда происходит поворот вокруг собственной оси
Тогда возникает ошибка Exception 