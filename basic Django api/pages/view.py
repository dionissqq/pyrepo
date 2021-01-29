from django.http import HttpResponse
from django.shortcuts import render

def about_view(request, *args, **kargs):
    return HttpResponse('<h1>About page</h1>') 