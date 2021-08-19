package com.example.domain.response

sealed class UseCaseResult<out T : Any> {
    open class Success<out T : Any>(val data: T) : UseCaseResult<T>()
    open class Failure(val exception: Exception) : UseCaseResult<Nothing>()
}