package com.example.proyecto_final.ui.fragments

import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.viewModelScope
import com.example.proyecto_final.BaseViewModel
import com.example.proyecto_final.ui.Address
import com.example.proyecto_final.ui.Event
import com.example.domain.response.Coordinates
import com.example.domain.response.PositionMultipleLines
import com.example.domain.response.RecorridoBaseInformation
import com.example.domain.response.UseCaseResult
import com.example.domain.usecase.GetBaseRoutesBusesUseCase
import com.example.domain.usecase.GetLinesBusesUseCase
import com.example.domain.usecase.GetRecorridoEntrePuntosSeleccionados
import com.google.android.gms.maps.model.LatLng
import com.google.android.gms.maps.model.Marker
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.delay
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import org.koin.core.KoinComponent
import org.koin.core.inject

class MapFragmentViewModel :
    BaseViewModel(), KoinComponent {

    private val getBaseRoutesBusesUseCase: GetBaseRoutesBusesUseCase by inject()
    private val getLinesBusesUseCase: GetLinesBusesUseCase by inject()
    private val getRecorridoEntrePuntosSeleccionados: GetRecorridoEntrePuntosSeleccionados by inject()

    val mapMutableLiveData = MutableLiveData<Event<Data>>()
    val liveData: MutableLiveData<Event<Data>>
        get() {
            return mapMutableLiveData
        }
    var visibleOptions: Boolean = false
    var checkLocation: Boolean = true
    var userAwaitBusRoutes: Boolean = false
    private val listMarkers = mutableListOf<Marker>()
    private val listSimulateBusMarkers = mutableListOf<Marker>()

    var activeLine: MutableList<Int> =
        mutableListOf() // Por defecto, al realizar nua busqueda, toma la linea 500
    var activeLineButtonFlag1: Boolean = false
    var activeLineButtonFlag2: Boolean = false
    var activeLineButtonFlag3: Boolean = false
    var activeLineButtonFlag4: Boolean = false
    var activeLineButtonFlag5: Boolean = false
    var activeLineButtonFlag6: Boolean = false

    var listRecorridoIda = mutableListOf<RecorridoBaseInformation>()
    var listActiveBusRecA = mutableListOf<Coordinates>()
    var listOriginalBusRecA = mutableListOf<Coordinates>()
    var listRecorridoVuelta = mutableListOf<RecorridoBaseInformation>()
    var listOriginalBusRecB = mutableListOf<Coordinates>()
    var listActiveBusRecB = mutableListOf<Coordinates>()

    var activeAlgorithm: String =
        RegressionAlgType.REGRESSION_LINEAL_MULTIPLE.toString() // Por defecto tomara el 1er algoritmo

    var myLocation: LatLng = LatLng(0.0, 0.0)

    lateinit var addressOrigin: Address
    lateinit var addressDestination: Address

    fun setLoading() {

        viewModelScope.launch {
            when (val result =
                withContext(Dispatchers.IO) { getLinesBusesUseCase.invoke() }) {
                is UseCaseResult.Failure -> {
                    mapMutableLiveData.postValue(
                        Event(
                            Data(
                                status = Status.ERROR,
                                data = result.exception.message,
                                dataAlternativa = "Back"
                            )
                        )
                    )
                }
                is UseCaseResult.Success -> {

                    mapMutableLiveData.postValue(
                        Event(
                            Data(
                                status = Status.LOADING,
                                data = result.data
                            )
                        )
                    )
                }
            }
        }
    }

    fun showBaseRoute(line: Int) {
        activeLine = mutableListOf(line)

        if (listRecorridoIda.isNotEmpty() && listActiveBusRecB.isNotEmpty()) {
            listRecorridoIda.clear()
            listActiveBusRecA.clear()
            listRecorridoVuelta.clear()
            listActiveBusRecB.clear()
        }

        launch {
            when (val result =
                withContext(Dispatchers.IO) { getBaseRoutesBusesUseCase.invoke(line) }) {
                is UseCaseResult.Failure -> {
                    mapMutableLiveData.postValue(
                        Event(
                            Data(
                                status = Status.ERROR,
                                data = result.exception.message,
                                dataAlternativa = "Back"
                            )
                        )
                    )
                }
                is UseCaseResult.Success -> {
                    val recorridoIda = RecorridoBaseInformation(
                        result.data[0].recorridoId,
                        result.data[0].linea,
                        result.data[0].coordenadas
                    )
                    val recorridoVuelta = RecorridoBaseInformation(
                        result.data[1].recorridoId,
                        result.data[1].linea,
                        result.data[1].coordenadas
                    )
                    listRecorridoIda.add(recorridoIda)
                    listRecorridoVuelta.add(recorridoVuelta)
                    selectActiveBus(listRecorridoIda, listRecorridoVuelta)

                    checkLocation = true
                    showAutoLocation()
                }
            }
        }
    }

    private fun selectActiveBus(
        recorridoIda: MutableList<RecorridoBaseInformation>,
        recorridoVuelta: MutableList<RecorridoBaseInformation>
    ) {
        listActiveBusRecA.addAll(recorridoIda[0].coordenadas)
        listActiveBusRecB.addAll(recorridoVuelta[0].coordenadas)
        listOriginalBusRecA.addAll(recorridoIda[0].coordenadas)
        listOriginalBusRecB.addAll(recorridoVuelta[0].coordenadas)
    }

    private fun checkBothFields() {
        if (::addressOrigin.isInitialized && ::addressDestination.isInitialized) {
            if (addressOrigin.name.isNotEmpty() && addressDestination.name.isNotEmpty()) {
                mapMutableLiveData.value = Event(
                    Data(status = Status.ACTIVATE_BUTTON)
                )
            } else {
                mapMutableLiveData.value = Event(
                    Data(
                        status = Status.DEACTIVATE_BUTTON
                    )
                )
            }
        } else {
            mapMutableLiveData.value = Event(
                Data(status = Status.DEACTIVATE_BUTTON)
            )
        }
    }


    fun setManualOriginPoint(
        address: Address
    ) {

        addressOrigin = address
        mapMutableLiveData.postValue(
            Event(
                Data(
                    status = Status.MANUAL_POINT,
                    data = address
                )
            )
        )
        checkBothFields()
    }

    fun setManualDestPoint(
        address: Address
    ) {

        addressDestination = address
        mapMutableLiveData.postValue(
            Event(
                Data(
                    status = Status.MANUAL_POINT,
                    data = address
                )
            )
        )

        checkBothFields()
    }

    fun proceedSearching() {
        checkLocation = false
        if (activeLine.isNotEmpty()) {
            getRecorridoEntreDosPuntosSeleccionados(
                PositionMultipleLines(
                    Coordinates(addressOrigin.latitude!!, addressOrigin.longitude!!),
                    Coordinates(addressDestination.latitude!!, addressDestination.longitude!!),
                    activeLine
                )
            )
        } else {

            mapMutableLiveData.postValue(
                Event(
                    Data(
                        status = Status.ERROR,
                        data = "No hay ninguna linea seleccionada para la busqueda, Por favor seleccione una desde el Menu de Configuración",
                        dataAlternativa = "Back"
                    )
                )
            )
        }
    }

    fun addMarker(marker: Marker) {
        listMarkers.add(marker)
    }

    fun addSimBusMarker(markerA: Marker, markerB: Marker) {
        listSimulateBusMarkers.clear()
        listSimulateBusMarkers.add(markerA)
        listSimulateBusMarkers.add(markerB)
    }

    fun cleanMarkers() {
        listMarkers.clear()
        addressOrigin = Address()
        addressDestination = Address()
        checkLocation = false
        mapMutableLiveData.postValue(
            Event(
                Data(status = Status.DEACTIVATE_BUTTON)
            )
        )
    }

    fun showAutoLocation() {
        if (checkLocation && activeLine.isNotEmpty()) {
            launch {
                withContext(Dispatchers.IO) {
                    delay(2000L) // retraso non-blocking de 4 segundos
                    if (listActiveBusRecA.size == 0 || listActiveBusRecB.size == 0) {
                        listActiveBusRecA = listOriginalBusRecA
                        listActiveBusRecB = listOriginalBusRecB
                    }

                    if (checkLocation) {
                        // Cuando tengo que mostrar el recorrido de un colectivo
                        // le paso la lista de recorrido ida y vuelta

                        mapMutableLiveData.postValue(
                            Event(
                                Data(
                                    status = Status.SHOW_LOC,
                                    data = listActiveBusRecA[0],
                                    dataAlternativa = listActiveBusRecB[0]
                                )
                            )
                        )
                        listActiveBusRecA.removeAt(0)
                        listActiveBusRecB.removeAt(0)
                    } else {
                        if (userAwaitBusRoutes) {
                            mapMutableLiveData.postValue(
                                Event(
                                    Data(
                                        status = Status.AWAY_MARKERS
                                    )
                                )
                            )
                        } else {
                            //nothing - continue

                        }

                    }
                }
            }
        }
    }

    private fun getRecorridoEntreDosPuntosSeleccionados(puntosSeleccionados: PositionMultipleLines) {

        viewModelScope.launch {
            when (val result =
                withContext(Dispatchers.IO) {
                    getRecorridoEntrePuntosSeleccionados.invoke(
                        puntosSeleccionados
                    )
                }) {
                is UseCaseResult.Failure -> {
                    mapMutableLiveData.postValue(
                        Event(
                            Data(
                                status = Status.ERROR,
                                data = result.exception.message,
                                dataAlternativa = "Back"
                            )
                        )
                    )
                }
                is UseCaseResult.Success -> {

                    mapMutableLiveData.postValue(
                        Event(
                            Data(
                                status = Status.PROCEED_SEARCHING,
                                data = result.data,
                                dataAlternativa = addressOrigin,
                                extraData = addressDestination
                            )
                        )
                    )

                    addressOrigin = Address()
                    addressDestination = Address()
                    checkBothFields()
                }
            }
        }
    }

    fun setLineSelectedByConfigurations(line: Int) {
        activeLine.add(line)
        showBaseRoute(line)
    }

    fun setAlgorithmSelectedByConfigurations(algorithm: String) {
        activeAlgorithm = algorithm
    }

    fun removeActiveLine() {
        activeLine.clear()
    }

    fun removedSelectedLinesByConfigurations() {
        activeLine.clear()

        mapMutableLiveData.postValue(
            Event(
                Data(
                    status = Status.CLEAN_MAP
                )
            )
        )
    }

    fun setGoToButton() {
        if (activeLineButtonFlag1 || activeLineButtonFlag2 || activeLineButtonFlag3 || activeLineButtonFlag4 || activeLineButtonFlag5 || activeLineButtonFlag6) {
            mapMutableLiveData.postValue(
                Event(
                    Data(
                        status = Status.ENABLE_GOTO_BUTTON
                    )
                )
            )
        } else {
            mapMutableLiveData.postValue(
                Event(
                    Data(
                        status = Status.DISABLE_GOTO_BUTTON
                    )
                )
            )
        }
    }

    data class Data(
        var status: Status,
        var data: Any? = null,
        var dataAlternativa: Any? = null,
        var extraData: Any? = null,
        var error: Exception? = null
    )

    enum class Status {
        LOADING,
        ERROR,
        SHOW_ROUTES,
        MANUAL_POINT,
        PROCEED_SEARCHING,
        ACTIVATE_BUTTON,
        DEACTIVATE_BUTTON,
        SHOW_LOC,
        ENABLE_GOTO_BUTTON,
        DISABLE_GOTO_BUTTON,
        AWAY_MARKERS,
        CLEAN_MAP
    }
}