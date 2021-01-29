from django.urls import path
from product.views import *

urlpatterns = [
    path('create',ProductCreateView.as_view()),
    path('all',ProductListView.as_view()),
    path('<int:pk>',ProductDetailView.as_view()),
]