extends Resource
class_name WeaponData

@export var cooldown: float = 1
@export var damage: float = 3
@export var duration: float = 1.5
@export var speed: float = 250
@export var knockback: float = 250
@export var pierce: int = 1
@export var size: float = 1
@export var amount: float = 0

@export var title: String = "Weapon"
@export var level: int = 0
@export var description: String = "Starting weapon"
@export var icon: CompressedTexture2D

@export var next: WeaponData
