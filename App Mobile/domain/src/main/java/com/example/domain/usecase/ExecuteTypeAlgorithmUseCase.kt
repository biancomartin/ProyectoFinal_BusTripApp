package com.example.domain.usecase

import com.example.domain.response.TravelBody
import com.example.domain.response.UseCaseResult
import com.example.domain.services.AlgorithmsService
import org.koin.core.KoinComponent

class ExecuteTypeAlgorithmUseCase(private val getAlgorithmsServiceRepository: AlgorithmsService) :
    KoinComponent {

    fun selectTypeAlgService(
        listOfParadas: List<*>?,
        algoritm: String
    ): UseCaseResult<List<TravelBody>> = when (algoritm) {
        "Regresión Lineal Múltiple" -> {
            getAlgorithmsServiceRepository.getCalcularTiempoPorRegresionLinealMultiple(
                listOfParadas
            )
        }
        "Enfoque Matricial" -> getAlgorithmsServiceRepository.getCalcularTiempoPorRegresionEnfoqueMatricial(
            listOfParadas
        )
        else -> {
            UseCaseResult.Success(emptyList())
        }
    }

}