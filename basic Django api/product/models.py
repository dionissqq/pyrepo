from django.db import models
from django.core.validators import MaxValueValidator, MinValueValidator
from django.contrib.auth import get_user_model
User = get_user_model()

# Create your models here.
class Product(models.Model) :
    name = models.CharField(max_length=30)
    price = models.DecimalField(decimal_places=2, max_digits=100)
    quantity = models.IntegerField(
        default=0,
        validators=[
            MaxValueValidator(10000),
            MinValueValidator(0)
        ])
    description = models.TextField()
    user = models.ForeignKey(User, on_delete=models.CASCADE, null = True )
