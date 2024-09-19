extends CharacterBody2D

@onready var sprite: AnimatedSprite2D = $sprite

const SPEED = 80.0

func _physics_process(delta: float) -> void:
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
