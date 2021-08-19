package com.example.proyecto_final.ui.fragments

import android.annotation.SuppressLint
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.viewModelScope
import com.example.proyecto_final.BaseViewModel
import com.example.proyecto_final.ui.Event
import com.example.proyecto_final.ui.InfoPuntoParada
import com.example.proyecto_final.ui.toDomainModel
import com.example.domain.response.UseCaseResult
import com.example.domain.usecase.ExecuteTypeAlgorithmUseCase
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import org.koin.core.KoinComponent
import java.text.SimpleDateFormat
import java.util.*

class FragmentTravelPredictionViewModel(private val getCalculoAlgSimpleUseCase: ExecuteTypeAlgorithmUseCase) :
    BaseViewModel(), KoinComponent {

    lateinit var algorithm: String

    val predictionMutableLiveData = MutableLiveData<Event<Data>>()
    val liveData: MutableLiveData<Event<Data>>
        get() {
            return predictionMutableLiveData
        }

    @SuppressLint("SimpleDateFormat")
    fun getCurrentPrediction(
        listOfParadas: List<InfoPuntoParada>?,
        algoritmo: String
    ) {


        predictionMutableLiveData.postValue(
            Event(
                Data(
                    status = Status.LOADING
                )
            )
        )
        algorithm = algoritmo

        val date = Calendar.getInstance().time
        val dateInString = date.toString("yyyy-MM-dd'T'HH:mm:ss")
        listOfParadas?.forEach { it.fecha = dateInString }

        val listOfStoppingPlace = listOfParadas?.map { it.toDomainModel() }
        viewModelScope.launch {
            when (val result =
                withContext(Dispatchers.IO) {
                    getCalculoAlgSimpleUseCase.selectTypeAlgService(
                        listOfStoppingPlace,
                        algorithm
                    )
                }) {
                is UseCaseResult.Failure -> {
                    predictionMutableLiveData.postValue(
                        Event(
                            Data(
                                status = Status.ERROR,
                                data = result.exception.message,
                                dataAlternativa = "Volver"
                            )
                        )
                    )
                }
                is UseCaseResult.Success -> {
                    val travelData = result.data
                    if (travelData.isNotEmpty()) {
                        predictionMutableLiveData.postValue(
                            Event(
                                Data(
                                    status = Status.CONTENT_DATA,
                                    data = result.data
                                )
                            )
                        )
                    } else {
                        predictionMutableLiveData.postValue(
                            Event(
                                Data(
                                    status = Status.ERROR,
                                    data = "La consulta que realizaste no se ha encontrado.Por favor busque otra ubicación",
                                    dataAlternativa = "Volver"
                                )
                            )
                        )
                    }
                }
            }
        }
    }


    data class Data(
        var status: Status,
        var data: Any? = null,
        var dataAlternativa: Any? = null,
        var error: Exception? = null
    )

    enum class Status {
        LOADING,
        CONTENT_DATA,
        ERROR,
    }
}

fun Date.toString(format: String, locale: Locale = Locale.getDefault()): String {
    val formatter = SimpleDateFormat(format, locale)
    return formatter.format(this)
}