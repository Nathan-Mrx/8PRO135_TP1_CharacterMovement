
extends CharacterBody2D

@onready var sprite: AnimatedSprite2D = $sprite

const SPEED = 80.0

func _physics_process(_delta: float) -> void:
	var direction := Vector2(
		Input.get_axis("left", "right"),
		Input.get_axis("up", "down")
	).normalized()

	velocity = direction * SPEED

	if direction.x != 0:
		sprite.play("walk_right_left")
		sprite.flip_h = direction.x < 0
	elif direction.y > 0:
		sprite.play("walk_down")
	elif direction.y < 0:
		sprite.play("walk_up")
	else:
		sprite.play("idle")

	move_and_slide()
	
	if Input.is_action_just_pressed("Save"):
		CustomGameLoop.Get().GetSaveManager().SaveGame("res://save.json")
		
	if Input.is_action_just_pressed("Load"):
		CustomGameLoop.Get().GetSaveManager().LoadGame("res://save.json")

func Save():
	var save_dict = {
		"Filename" : get_scene_file_path(),
		"Parent" : get_parent().get_path(),
		"PosX" : position.x, # Vector2 is not supported by JSON  	
		"PosY" : position.y,
	}
	return save_dict
