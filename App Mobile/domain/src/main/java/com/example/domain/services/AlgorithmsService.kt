package com.example.domain.services

import com.example.domain.response.TravelBody
import com.example.domain.response.UseCaseResult

interface AlgorithmsService {

    fun getCalcularTiempoPorRegresionLinealMultiple(destination: List<*>?): UseCaseResult<List<TravelBody>>
    fun getCalcularTiempoPorRegresionEnfoqueMatricial(destination: List<*>?): UseCaseResult<List<TravelBody>>
}