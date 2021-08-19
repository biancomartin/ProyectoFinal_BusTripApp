package com.example.data.mapper

interface BaseMapper<E, D> {
    fun transform(type: E): D
}
