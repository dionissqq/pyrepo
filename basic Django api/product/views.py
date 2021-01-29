from rest_framework import generics
from product.serializer import *
from product.models import Product
from product.permission import IsOwnerOrReadOnly
# Create your views here.

class ProductCreateView(generics.CreateAPIView):
    serializer_class = ProductSerializer

class ProductListView(generics.ListAPIView):
    serializer_class = ProductListSerializer
    queryset = Product.objects.all()

class ProductDetailView(generics.RetrieveUpdateDestroyAPIView):
    serializer_class = ProductSerializer
    queryset = Product.objects.all()
    permission_classes = (IsOwnerOrReadOnly,)