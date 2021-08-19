package com.example.domain.usecase

import com.example.domain.response.PositionMultipleLines
import com.example.domain.services.RideService
import org.koin.core.KoinComponent

class GetListMultipleLinesTravelInfoUseCase(private val getRideServiceRepository: RideService) :
    KoinComponent {

    operator fun invoke(destination: PositionMultipleLines) =
        getRideServiceRepository.getMultipleLinesSearching(destination)
}