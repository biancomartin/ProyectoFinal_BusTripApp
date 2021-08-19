package com.example.domain.usecase

import com.example.domain.services.RideService
import org.koin.core.KoinComponent

class GetLinesBusesUseCase(private val getRideServiceRepository: RideService) : KoinComponent {

    operator fun invoke() = getRideServiceRepository.getLinesInformation()
}